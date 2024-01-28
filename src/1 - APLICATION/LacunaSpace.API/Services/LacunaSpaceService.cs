﻿using GameMasterEnterprise.Domain.Intefaces;
using LacunaSpace.API.Models;
using LacunaSpace.API.Models.Request;
using LacunaSpace.API.Models.Response;
using LacunaSpace.Domain.Intefaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LacunaSpace.Service
{
    public class LacunaSpaceService : BaseService, ILacunaSpaceService
    {
        private Dictionary<string, ProbeSyncInfoModel> ProbeSyncInfoCache = new Dictionary<string, ProbeSyncInfoModel>();

        public LacunaSpaceService(INotificador notificador, HttpClient httpClient)
            : base(notificador, httpClient)
        {

        }

        public async Task<string> IniciarTeste(StartRequestModel requestModel)
        {
            try
            {
                var response = await PostAsync<StartBaseResponseModel>("https://luma.lacuna.cc/api/start", requestModel);
                if (response.code == "Success")
                {
                    return response.accessToken;
                }
                else
                {
                    Notificar("Erro");
                    throw new Exception($"Falha ao iniciar o teste");
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao iniciar o teste: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ProbeModel>> ListarSondas(string accessToken)
        {
            try
            {
                var response = await GetWithTokenAsync<ProbeListResponseModel>("https://luma.lacuna.cc/api/probe", accessToken);

                if (response.code == "Success")
                {
                    return response.probes;
                }
                else
                {
                    Notificar("Erro ao listar sondas");
                    throw new Exception($"Falha ao listar sondas: {response.message}");
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao listar sondas: {ex.Message}");
                throw;
            }
        }


        public async Task SincronizarRelogios(ProbeListRequestModel sondas, string accessToken)
        {
            try
            {
                foreach (var sonda in sondas.probes)
                {
                    await SyncAndVerifyClockWithProbe(sonda.id, accessToken);
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro na sincronização de relógios: {ex.Message}");
                throw;
            }
        }
        public async Task SyncAndVerifyClockWithProbe(string probeId, string accessToken)
        {
            try
            {
                await SyncClockWithProbe(probeId, accessToken);
                VerificarSincronizacaoLocal(probeId);
            }
            catch (Exception ex)
            {
                Notificar($"Erro na sincronização com a sonda {probeId}: {ex.Message}");
                throw;
            }
        }
        private void VerificarSincronizacaoLocal(string probeId)
        {
            try
            {
                if (Math.Abs(ProbeSyncInfoCache[probeId].TimeOffset) > TimeSpan.FromMilliseconds(5).Ticks)
                {
                    Console.WriteLine($"O relógio da sonda {probeId} não está sincronizado!");
                }
                else
                {
                    Console.WriteLine($"O relógio da sonda {probeId} está sincronizado!");
                }
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao verificar a sincronização local para a sonda {probeId}: {ex.Message}");
                throw;
            }
        }

        public async Task SyncClockWithProbe(string probeId, string accessToken)
        {
            try
            {
                long t0 = DateTimeOffset.UtcNow.Ticks;
                var syncResponse = await SyncClockWithProbeApi(probeId, accessToken);
                long t3 = DateTimeOffset.UtcNow.Ticks;
                long timeOffset = ParseAndCalculateTimeOffset(syncResponse, t0, t3);
                long roundTrip = ParseAndCalculateRoundTrip(syncResponse, t0, t3);

                CacheTimeOffsetAndRoundTrip(probeId, timeOffset, roundTrip);
            }
            catch (Exception ex)
            {
                Notificar($"Erro na sincronização com a sonda {probeId}: {ex.Message}");
                throw;
            }
        }

        private async Task<SyncResponseModel> SyncClockWithProbeApi(string probeId, string accessToken)
        {
            var response = await PostWithTokenAsync<SyncResponseModel>(@$"https://luma.lacuna.cc/api/probe/{probeId}/sync",probeId, accessToken);

            if (response.code == "Success")
            {
                return response;
            }
            else
            {
                throw new Exception($"Falha na sincronização com a sonda {probeId}: {response.message}");
            }
        }

        private void CacheTimeOffsetAndRoundTrip(string probeId, long timeOffset, long roundTrip)
        {

            if (!ProbeSyncInfoCache.ContainsKey(probeId))
            {
                ProbeSyncInfoCache.Add(probeId, new ProbeSyncInfoModel());
            }

            ProbeSyncInfoCache[probeId].ProbeId = probeId;
            ProbeSyncInfoCache[probeId].TimeOffset = timeOffset;
            ProbeSyncInfoCache[probeId].RoundTrip = roundTrip;
        }

        private long ParseAndCalculateTimeOffset(SyncResponseModel syncResponse, long t0, long t3)
        {
            long t1 = long.Parse(syncResponse.t1);
            long t2 = long.Parse(syncResponse.t2);

            return (t1 - t0 + t2 - t3) / 2;
        }

        private long ParseAndCalculateRoundTrip(SyncResponseModel syncResponse, long t0, long t3)
        {
            long t1 = long.Parse(syncResponse.t1);
            long t2 = long.Parse(syncResponse.t2);

            return t3 - t0 - (t2 - t1);
        }
    }
}

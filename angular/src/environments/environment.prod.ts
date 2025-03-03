import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44367/',
  redirectUri: baseUrl,
  clientId: 'Microblog_App',
  responseType: 'code',
  scope: 'offline_access Microblog',
  requireHttps: true,
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Microblog',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44367',
      rootNamespace: 'Microblog',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge'
  }
} as Environment;

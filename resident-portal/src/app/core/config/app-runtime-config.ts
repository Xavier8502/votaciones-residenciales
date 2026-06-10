type RuntimeConfig = {
  apiBaseUrl?: string;
  hubBaseUrl?: string;
};

declare global {
  interface Window {
    __APP_CONFIG__?: RuntimeConfig;
  }
}

const trimTrailingSlash = (value?: string) =>
  value ? value.replace(/\/+$/, '') : '';

const runtimeConfig = window.__APP_CONFIG__ ?? {};

export const appRuntimeConfig = {
  apiBaseUrl: trimTrailingSlash(runtimeConfig.apiBaseUrl),
  hubBaseUrl: trimTrailingSlash(runtimeConfig.hubBaseUrl ?? runtimeConfig.apiBaseUrl)
};

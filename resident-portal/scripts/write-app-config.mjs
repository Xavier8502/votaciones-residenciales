import { writeFileSync } from 'node:fs';
import { resolve } from 'node:path';

const config = {
  apiBaseUrl: process.env.API_BASE_URL ?? '',
  hubBaseUrl: process.env.HUB_BASE_URL ?? process.env.API_BASE_URL ?? ''
};

writeFileSync(
  resolve('public/app-config.js'),
  `window.__APP_CONFIG__ = ${JSON.stringify(config, null, 2)};\n`,
  'utf8'
);

console.log('app-config.js generated for resident-portal');

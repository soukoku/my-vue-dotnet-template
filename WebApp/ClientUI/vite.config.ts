import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import fs from 'fs'
import path from 'path'
import child_process from 'child_process'
import pkg from './package.json' assert { type: 'json' }

const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ''
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`

const certificateName = pkg.name || 'vueapp.client'
const certFilePath = path.join(baseFolder, `${certificateName}.pem`)
const keyFilePath = path.join(baseFolder, `${certificateName}.key`)

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  if (
    0 !==
    child_process.spawnSync(
      'dotnet',
      ['dev-certs', 'https', '--export-path', certFilePath, '--format', 'Pem', '--no-password', '--trust'],
      { stdio: 'inherit' }
    ).status
  ) {
    throw new Error('Could not create certificate.')
  }
}

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  server: {
    https: {
      key: fs.readFileSync(keyFilePath),
      cert: fs.readFileSync(certFilePath)
    },
    port: 3000,
    hmr: { host: 'localhost', clientPort: 3000 }
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  build: {
    manifest: true,
    // outDir: '../app/',
    // emptyOutDir: true,
    rollupOptions: {
      // specify all the custom entry pages
      input: ['src/entry-pages/home.ts'],
      output: {
        manualChunks: (id: string) => {
          if (id.includes('a-very-large-dependency')) {
            return 'big-chungus'
          }

          if (id.includes('node_modules')) {
            return 'vendors'
          }
        }
      }
    }
  }
})

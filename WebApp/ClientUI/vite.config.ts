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
      [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
        '--trust'
      ],
      { stdio: 'inherit' }
    ).status
  ) {
    throw new Error('Could not create certificate.')
  }
}

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  base: process.env.NODE_ENV === 'development' ? '/' : '/template/',
  server: {
    https: {
      key: fs.readFileSync(keyFilePath),
      cert: fs.readFileSync(certFilePath)
    },
    // keep the port number in sync with what's in Program.cs value
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
      // specify all the custom entry pages to be used
      input: [
        'src/entry-pages/home.ts',
        'src/entry-pages/admin.ts',
        'src/entry-pages/docs.ts',
        'src/entry-pages/error.ts'
      ],
      output: {
        manualChunks: (id: string) => {
          if (id.includes('a-very-large-dependency')) {
            return 'big-chungus'
          }
          if (id.includes('swagger')) {
            return 'vendor-swagger'
          }
          if (id.includes('ag-grid-vue3') || id.includes('ag-grid-community'))
            return 'vendor-ag-grid'

          if (id.includes('node_modules')) {
            return 'vendors'
          }
        }
      }
    }
  }
})

import typography from '@tailwindcss/typography'
import forms from '@tailwindcss/forms'
import colors from 'tailwindcss/colors'
import defaultTheme from 'tailwindcss/defaultTheme'

/** @type {import('tailwindcss').Config} */
export default {
  content: ['./src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        blue: colors.sky,
        gray: colors.zinc
      },
      fontFamily: {
        sans: ['Inter Variable', ...defaultTheme.fontFamily.sans]
      },
      fontSize: {
        sm2: ['0.8125rem', '1.125rem'],
        xs2: ['0.6875rem', '0.875rem']
      },
      rotate: {
        270: '270deg;',
        '-270': '-270deg'
      }
    }
  },
  plugins: [typography, forms]
}

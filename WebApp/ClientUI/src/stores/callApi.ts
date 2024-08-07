export const csrfToken = (
  // name of csrf-token is set in Program.cs
  document.querySelector('input[type=hidden][name=csrf-token]') as HTMLInputElement
)?.value

/**
 * Quick non-throwing way to call api via fetch. If error occurs then the returned
 * object will have a message property containing the error just like when it's
 * returned from the server.
 * @param {string} method
 * @param {string} url
 * @param {*} payload
 */
export async function callApi<T>(
  method: 'GET' | 'POST' | 'PUT' | 'DELETE',
  url: string,
  payload: any = undefined
): Promise<T> {
  const headers = {
    'X-TIMEZONE': Intl.DateTimeFormat().resolvedOptions().timeZone,
    'X-CSRF-TOKEN': method === 'GET' ? undefined : csrfToken
  } as Record<string, string>
  const isForm = payload instanceof FormData
  if (payload && !isForm) {
    headers['Content-Type'] = 'application/json'
  }
  const fetchReq = {
    method,
    headers,
    body: payload ? (isForm ? payload : JSON.stringify(payload)) : undefined
  }
  try {
    const resp = await fetch(url, fetchReq)
    if (resp.status === 401) {
      window.location.reload()
      return { message: 'Unauthorized' } as T
    }
    if (resp.headers.get('Content-Type')?.startsWith('application/json'))
      return (await resp.json()) as T
    return resp as T
  } catch (err: any) {
    console.error(`${fetchReq.method} ${url}`, err)
    const message = (typeof err === 'string' ? err : err.message) || 'Unknown error'
    return { message } as T
  }
}

const utf8FilenameRegex = /filename\*=UTF-8''([\w%\-.]+)(?:; ?|$)/i
const asciiFilenameRegex = /^filename=(["']?)(.*?[^\\])\1(?:; ?|$)/i

/**
 * Get file name from content disposition header.
 * @param disposition
 * @returns
 */
export function getFileName(disposition: string | null): string {
  // from https://stackoverflow.com/questions/40939380/how-to-get-file-name-from-content-disposition
  let fileName: string = ''
  if (disposition) {
    if (utf8FilenameRegex.test(disposition)) {
      const matches = utf8FilenameRegex.exec(disposition)
      if (matches != null && matches[1]) {
        fileName = decodeURIComponent(matches[1])
      }
    } else {
      // prevent ReDos attacks by anchoring the ascii regex to string start and
      //  slicing off everything before 'filename='
      const filenameStart = disposition.toLowerCase().indexOf('filename=')
      if (filenameStart >= 0) {
        const partialDisposition = disposition.slice(filenameStart)
        const matches = asciiFilenameRegex.exec(partialDisposition)
        if (matches != null && matches[2]) {
          fileName = matches[2]
        }
      }
    }
  }
  return fileName
}

/**
 * Saves fetched response as a file.
 * @param response response from fetch call
 * @param fallbackFileName Name of the file to use if filename from response is empty.
 */
export async function saveResponseAsFile(response: Response, fallbackFileName: string) {
  const disp = response.headers.get('Content-Disposition')
  const a = document.createElement('a')
  a.href = URL.createObjectURL(await response.blob())
  a.download = getFileName(disp) || fallbackFileName
  a.click()
}

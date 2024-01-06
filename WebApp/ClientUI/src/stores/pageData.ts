const json = document.getElementById('pageData')?.textContent
const pageData = JSON.parse(json || 'null')

/**
 * Gets the object defined in the #pageData script node
 * on the page.
 * @returns page data as specified object shape.
 */
export function getPageData<T>() {
  return pageData as T
}

const csrfToken = (
  document.querySelector('input[type=hidden][name=csrf-token]') as HTMLInputElement
)?.value
/**
 * Gets the CSRF token that should be used for modification-type requests.
 * @returns csrf token value
 */
export function getCsrfToken() {
  return csrfToken
}

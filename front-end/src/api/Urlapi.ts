import axios from 'axios';
import toast from 'react-hot-toast';

const BASE_DOMAIN =
  import.meta.env.VITE_API_URL ||
  (import.meta.env.DEV ? 'http://localhost:5272' : '');

export const apiClient = axios.create({
  baseURL: `${BASE_DOMAIN}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 429) {
      toast.error("Whoa there! You're moving too fast. Please wait a minute.", {
        id: 'rate-limit',
        duration: 4000,
      });
    }

    if (error.response?.status === 401) {
      toast.error('Your session has expired. Please log in again.');
    }

    return Promise.reject(error);
  }
);

export interface ShortenUrlRequest {
  Url: string;
}

export async function CreateShortUrl(dto: ShortenUrlRequest): Promise<string> {
  try {
    const response = await apiClient.post('/Url/CreateShortUrl', dto);
    return response.data.shortUrl;
  } catch (error: any) {
    throw new Error(
      error?.response?.data?.error ||
      error?.response?.data?.message ||
      'Failed to shorten URL'
    );
  }
}

export function ReturnUrl(shortUrl: string) {
  const code = shortUrl.split('/').pop();
  window.location.href = `${BASE_DOMAIN}/${code}`;
}
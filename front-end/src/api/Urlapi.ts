const Base_URL = "http://localhost:5245/api/Url";
export interface ShortenUrlRequest {
  Url: string;
}

export async function CreateShortUrl(dto: ShortenUrlRequest): Promise<string> {
  try {
    const response = await fetch(`${Base_URL}/CreateShortUrl`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(dto),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.error || "Failed to shorten URL");
    }

    const data = await response.json();
    return data.shortUrl; // Backend returns { "shortUrl": "http://..." }
  } catch (error) {
    if (error instanceof Error) {
      throw error;
    }
    throw new Error("Network error");
  }
}

export function ReturnUrl(shortUrl: string) {
  const code = shortUrl.split('/').pop();
  window.location.href = `${Base_URL}/${code}`;
}
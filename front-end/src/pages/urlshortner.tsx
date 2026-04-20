import { useState } from "react";
import { CreateShortUrl } from "../api/Urlapi";
import type { ShortenUrlRequest } from "../api/Urlapi";

export default function UrlShortener() {
  const [url, setUrl] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [shortUrl, setShortUrl] = useState<string>("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!url.trim()) return;

    setLoading(true);
    setError(null);
    setShortUrl("");

    try {
      const request: ShortenUrlRequest = { Url: url.trim() };
      const result = await CreateShortUrl(request);
      setShortUrl(result);
      setUrl("");
    } catch (err) {
      setError(err instanceof Error ? err.message : "An unexpected error occurred");
    } finally {
      setLoading(false);
    }
  };

  const copyToClipboard = async () => {
    try {
      await navigator.clipboard.writeText(shortUrl);
      alert("Copied to clipboard!");
    } catch {
      alert("Failed to copy");
    }
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        width: "100%",
        background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
        fontFamily: "system-ui, -apple-system, sans-serif",

        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      {/* Centered Container */}
      <div
        style={{
          width: "100%",
          maxWidth: "1100px",
          padding: "40px",
          display: "flex",
          flexDirection: "column",
        }}
      >
        {/* Header */}
        <div style={{ textAlign: "center", marginBottom: "40px" }}>
          <h1
            style={{
              fontSize: "80px",
              fontWeight: "900",
              color: "white",
              margin: "0 0 20px 0",
              letterSpacing: "-2px",
            }}
          >
            URL Shortener
          </h1>
          <p
            style={{
              color: "rgba(255, 255, 255, 0.95)",
              fontSize: "24px",
              margin: 0,
              fontWeight: "400",
            }}
          >
            Transform long URLs into short, shareable links instantly
          </p>
        </div>

        {/* Main Content */}
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            gap: "40px",
          }}
        >
          {/* Input Card */}
          <div
            style={{
              background: "white",
              borderRadius: "24px",
              padding: "60px 80px",
              boxShadow: "0 25px 50px -12px rgba(0, 0, 0, 0.4)",
            }}
          >
            <form onSubmit={handleSubmit}>
              <div style={{ marginBottom: "30px" }}>
                <label
                  style={{
                    display: "block",
                    fontSize: "20px",
                    fontWeight: "600",
                    color: "#1f2937",
                    marginBottom: "15px",
                  }}
                >
                  Paste your long URL here
                </label>
                <input
                  type="url"
                  value={url}
                  onChange={(e) => setUrl(e.target.value)}
                  placeholder="https://example.com/your-very-long-url-that-needs-shortening"
                  required
                  style={{
                    width: "100%",
                    padding: "22px 28px",
                    fontSize: "20px",
                    border: "3px solid #e5e7eb",
                    borderRadius: "14px",
                    outline: "none",
                    transition: "all 0.2s",
                    boxSizing: "border-box",
                    fontFamily: "inherit",
                  }}
                  onFocus={(e) => {
                    e.target.style.borderColor = "#667eea";
                    e.target.style.boxShadow = "0 0 0 5px rgba(102, 126, 234, 0.15)";
                  }}
                  onBlur={(e) => {
                    e.target.style.borderColor = "#e5e7eb";
                    e.target.style.boxShadow = "none";
                  }}
                />
              </div>

              <button
                type="submit"
                disabled={loading || !url.trim()}
                style={{
                  width: "100%",
                  padding: "24px",
                  fontSize: "22px",
                  fontWeight: "700",
                  color: "white",
                  background: loading || !url.trim() ? "#9ca3af" : "#667eea",
                  border: "none",
                  borderRadius: "14px",
                  cursor: loading || !url.trim() ? "not-allowed" : "pointer",
                  transition: "all 0.3s",
                  boxShadow:
                    loading || !url.trim()
                      ? "none"
                      : "0 8px 20px rgba(102, 126, 234, 0.4)",
                }}
                onMouseEnter={(e) => {
                  if (!loading && url.trim()) {
                    e.currentTarget.style.background = "#5568d3";
                    e.currentTarget.style.transform = "translateY(-3px)";
                    e.currentTarget.style.boxShadow =
                      "0 12px 28px rgba(102, 126, 234, 0.5)";
                  }
                }}
                onMouseLeave={(e) => {
                  if (!loading && url.trim()) {
                    e.currentTarget.style.background = "#667eea";
                    e.currentTarget.style.transform = "translateY(0)";
                    e.currentTarget.style.boxShadow =
                      "0 8px 20px rgba(102, 126, 234, 0.4)";
                  }
                }}
              >
                {loading ? "⏳ Shortening..." : "✨ Shorten URL"}
              </button>
            </form>

            {/* Error Message */}
            {error && (
              <div
                style={{
                  marginTop: "35px",
                  padding: "28px",
                  background: "#fef2f2",
                  border: "3px solid #fecaca",
                  borderRadius: "14px",
                }}
              >
                <div
                  style={{
                    fontSize: "18px",
                    fontWeight: "700",
                    color: "#991b1b",
                    marginBottom: "8px",
                  }}
                >
                  ✗ Error
                </div>
                <div style={{ fontSize: "17px", color: "#dc2626" }}>{error}</div>
              </div>
            )}
          </div>

          {/* Result Card */}
          {shortUrl && !error && (
            <div
              style={{
                background: "linear-gradient(135deg, #d1fae5 0%, #a7f3d0 100%)",
                borderRadius: "24px",
                padding: "60px 80px",
                border: "4px solid #10b981",
                boxShadow: "0 25px 50px -12px rgba(16, 185, 129, 0.4)",
              }}
            >
              <div
                style={{
                  fontSize: "28px",
                  fontWeight: "900",
                  color: "#065f46",
                  marginBottom: "30px",
                  display: "flex",
                  alignItems: "center",
                  gap: "15px",
                }}
              >
                <span style={{ fontSize: "40px" }}>✓</span>
                Success! Here's your shortened URL:
              </div>

              <div
                style={{
                  background: "white",
                  padding: "32px 40px",
                  borderRadius: "14px",
                  border: "3px solid #6ee7b7",
                  marginBottom: "30px",
                  boxShadow: "0 4px 12px rgba(0, 0, 0, 0.1)",
                }}
              >
                <div
                  style={{
                    fontSize: "32px",
                    fontWeight: "700",
                    color: "#667eea",
                    wordBreak: "break-all",
                    fontFamily: "monospace",
                    letterSpacing: "1px",
                  }}
                >
                  {shortUrl}
                </div>
              </div>

              <div style={{ display: "flex", gap: "25px" }}>
                <button
                  onClick={copyToClipboard}
                  style={{
                    flex: 1,
                    padding: "24px",
                    fontSize: "20px",
                    fontWeight: "700",
                    color: "white",
                    background: "#10b981",
                    border: "none",
                    borderRadius: "14px",
                    cursor: "pointer",
                    transition: "all 0.3s",
                    boxShadow: "0 8px 20px rgba(16, 185, 129, 0.4)",
                  }}
                  onMouseEnter={(e) => {
                    e.currentTarget.style.background = "#059669";
                    e.currentTarget.style.transform = "translateY(-3px)";
                    e.currentTarget.style.boxShadow =
                      "0 12px 28px rgba(16, 185, 129, 0.5)";
                  }}
                  onMouseLeave={(e) => {
                    e.currentTarget.style.background = "#10b981";
                    e.currentTarget.style.transform = "translateY(0)";
                    e.currentTarget.style.boxShadow =
                      "0 8px 20px rgba(16, 185, 129, 0.4)";
                  }}
                >
                  📋 Copy to Clipboard
                </button>

                <button
                  onClick={() => window.open(shortUrl, '_blank')}
                  style={{
                    flex: 1,
                    padding: "24px",
                    fontSize: "20px",
                    fontWeight: "700",
                    color: "#667eea",
                    background: "white",
                    border: "4px solid #667eea",
                    borderRadius: "14px",
                    cursor: "pointer",
                    transition: "all 0.3s",
                  }}
                  onMouseEnter={(e) => {
                    e.currentTarget.style.background = "#667eea";
                    e.currentTarget.style.color = "white";
                    e.currentTarget.style.transform = "translateY(-3px)";
                    e.currentTarget.style.boxShadow =
                      "0 8px 20px rgba(102, 126, 234, 0.4)";
                  }}
                  onMouseLeave={(e) => {
                    e.currentTarget.style.background = "white";
                    e.currentTarget.style.color = "#667eea";
                    e.currentTarget.style.transform = "translateY(0)";
                    e.currentTarget.style.boxShadow = "none";
                  }}
                >
                  🔗 Test Link
                </button>
              </div>
            </div>
          )}
        </div>

        {/* Footer */}
        <div
          style={{
            textAlign: "center",
            marginTop: "40px",
            color: "rgba(255, 255, 255, 0.95)",
            fontSize: "18px",
            fontWeight: "500",
          }}
        >
          Fast, simple, and secure URL shortening
        </div>
      </div>
    </div>
  );
}

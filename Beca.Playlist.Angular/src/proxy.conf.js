const PROXY_CONFIG = [
  {
    context: [
      "/",
    ],
    target: "https://localhost:7035",
    secure: false
  }
]

module.exports = PROXY_CONFIG;

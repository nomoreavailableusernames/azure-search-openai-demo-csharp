// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts("./service-worker-assets.js");
self.addEventListener("install", (event) => event.waitUntil(onInstall(event)));
self.addEventListener("activate", (event) =>
  event.waitUntil(onActivate(event))
);
self.addEventListener("fetch", () => {});

const cacheNamePrefix = "offline-cache-";
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [
  /\.dll$/,
  /\.pdb$/,
  /\.wasm/,
  /\.html/,
  /\.js$/,
  /\.json$/,
  /\.css$/,
  /\.woff$/,
  /\.png$/,
  /\.jpe?g$/,
  /\.gif$/,
  /\.ico$/,
  /\.blat$/,
  /\.dat$/,
];
const offlineAssetsExclude = [/^service-worker\.js$/];

async function onInstall(event) {
  console.info("Service worker: Install");

  // Fetch and cache all matching items from the assets manifest
  const assetsRequests = self.assetsManifest.assets
    .filter((asset) =>
      offlineAssetsInclude.some((pattern) => pattern.test(asset.url))
    )
    .filter(
      (asset) =>
        !offlineAssetsExclude.some((pattern) => pattern.test(asset.url))
    )
    .map((asset) => new Request(asset.url, { cache: "no-cache" }));
  await caches.open(cacheName).then((cache) => cache.addAll(assetsRequests));
}

async function onActivate(event) {
  console.info("Service worker: Activate");

  // Delete unused caches
  const cacheKeys = await caches.keys();
  await Promise.all(
    cacheKeys
      .filter((key) => key.startsWith(cacheNamePrefix) && key !== cacheName)
      .map((key) => caches.delete(key))
  );
}

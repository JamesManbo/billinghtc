'use strict';
const MANIFEST = 'flutter-app-manifest';
const TEMP = 'flutter-temp-cache';
const CACHE_NAME = 'flutter-app-cache';
const RESOURCES = {
  "assets/AssetManifest.json": "dbe1e47d9803b68a068f17b2ebf58546",
"assets/assets/default_avatar.svg": "6fdb38dc3bd13d4015ae3da480a32f6f",
"assets/assets/icons/ic_blue_notification.svg": "b6fa122a06436f7b6a49bb48eaae794d",
"assets/assets/icons/ic_coin.svg": "80f009bc04bb9528133265a4fe0feb82",
"assets/assets/icons/ic_contract.svg": "f210d4fd61e543296d47b0df85c59f88",
"assets/assets/icons/ic_email_circle.svg": "229a1179de889f212833592e002dee53",
"assets/assets/icons/ic_face.svg": "9eae1dada139551c2431633a52df18f5",
"assets/assets/icons/ic_location.svg": "49a92bcdfe06eadf6401eaf9927202e1",
"assets/assets/icons/ic_menu_avatar.svg": "d16050f4c534816f0f3854bbc63973c3",
"assets/assets/icons/ic_messenger.svg": "4d8c52b7406bb7834e0eba8433664862",
"assets/assets/icons/ic_phone_circle.svg": "1ca8f25ce5621bf34cc34eb39043aa58",
"assets/assets/icons/ic_service_in_use.svg": "ec02940b8cbb53b2504859c09db5d226",
"assets/assets/icons/ic_shape.svg": "0b238dfc5805110d9dcde5c9eed712ae",
"assets/assets/icons/ic_support.svg": "191b201973674598a7a9dcea2ce7b56b",
"assets/assets/icons/ic_user.svg": "625b6e018904b734004cd2e5187f2a41",
"assets/assets/icons/ic_white_notification.svg": "e7ffd9121ae434f92b0a1574e9bcfef7",
"assets/assets/icons/ic_white_phone.svg": "9b74a9e6cbf257d7ef7daa3cd461668c",
"assets/assets/icons/logo.png": "473c0ad89d063870de29be40dae558f6",
"assets/assets/ic_email.png": "06d167585627195ad49c29c0462c0b3e",
"assets/assets/ic_home_wifi.png": "646d50606aeec3f592552a5057992bd7",
"assets/assets/ic_lock.png": "e63687a84c7afc8ea7b3ba2dc85c0560",
"assets/assets/ic_logout.png": "2c8432c437943f619cf64068a248883c",
"assets/assets/ic_map.svg": "567413d62cf8bc8e6940648e9b46b644",
"assets/assets/ic_momo.svg": "597bb1b9fae9c36b75465a7e2f16f7de",
"assets/assets/ic_phone.png": "f48cd84d71d9ddfd1f5d351662259ec0",
"assets/assets/ic_receipt.svg": "0176fe35190b51812e9cd8c7bdfc97c9",
"assets/assets/ic_star.png": "b3534450099e37bc9f4f78312c5a59f8",
"assets/assets/ic_visa.svg": "5c0d28db5efbfc546859e717a94ac0bd",
"assets/assets/ic_vnpay.svg": "d71b32d1a4c09b0cb07f3a81f70352ca",
"assets/assets/item_contract.png": "d73bbfb09c4361c6cb33c04f48279e1e",
"assets/assets/item_contract.svg": "ad35781f6a38481c7c853ce681e2e12f",
"assets/assets/item_list.svg": "10f45c872a8abb99c85633cf0bb00d25",
"assets/assets/item_location.png": "07738b7a3451d6c8900ce25756abc655",
"assets/assets/item_location.svg": "263cc4e349e553b501888a463079e8b7",
"assets/assets/item_message.png": "88e459ab1f292831e72e5d77590a15de",
"assets/assets/item_message.svg": "785a0cfd8e65a90d34bcbbd214e018d0",
"assets/assets/item_payment.svg": "6cc9d38a28b8f4cc4f506067d6f1fa7c",
"assets/assets/item_promotion.png": "3b35e380bb85ab80f16c10d677221d63",
"assets/assets/item_promotion.svg": "be3395d7cf7d5c1dc9bd2397d04abea8",
"assets/assets/item_receipt.png": "c6288cc5dfd5021ad7726e9bf226d029",
"assets/assets/item_service.png": "aec2984ac21719a072fed743b88b2c7e",
"assets/assets/item_service.svg": "997be6187fee3e870b66f6f4ae02a650",
"assets/assets/item_service_in_use.png": "cdb1d47af6366a10e7c538ecfd29696d",
"assets/assets/place_holder_default.png": "59e11f34371c28e4222249dbc1e1b2ce",
"assets/assets/place_holder_default.svg": "3b9664a2984c71ae42494dad300defbd",
"assets/FontManifest.json": "71a4a82de411f155107da3f8dac64ebd",
"assets/fonts/MaterialIcons-Regular.otf": "4e6447691c9509f7acdbf8a931a85ca1",
"assets/NOTICES": "ee18a9b1fbbb33f73e79dedb3b125de2",
"assets/packages/cupertino_icons/assets/CupertinoIcons.ttf": "6d342eb68f170c97609e9da345464e5e",
"assets/packages/fluttertoast/assets/toastify.css": "a85675050054f179444bc5ad70ffc635",
"assets/packages/fluttertoast/assets/toastify.js": "e7006a0a033d834ef9414d48db3be6fc",
"assets/packages/flutter_inappwebview/t_rex_runner/t-rex.css": "5a8d0222407e388155d7d1395a75d5b9",
"assets/packages/flutter_inappwebview/t_rex_runner/t-rex.html": "16911fcc170c8af1c5457940bd0bf055",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_AMS-Regular.ttf": "657a5353a553777e270827bd1630e467",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Caligraphic-Bold.ttf": "a9c8e437146ef63fcd6fae7cf65ca859",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Caligraphic-Regular.ttf": "7ec92adfa4fe03eb8e9bfb60813df1fa",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Fraktur-Bold.ttf": "46b41c4de7a936d099575185a94855c4",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Fraktur-Regular.ttf": "dede6f2c7dad4402fa205644391b3a94",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Main-Bold.ttf": "9eef86c1f9efa78ab93d41a0551948f7",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Main-BoldItalic.ttf": "e3c361ea8d1c215805439ce0941a1c8d",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Main-Italic.ttf": "ac3b1882325add4f148f05db8cafd401",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Main-Regular.ttf": "5a5766c715ee765aa1398997643f1589",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Math-BoldItalic.ttf": "946a26954ab7fbd7ea78df07795a6cbc",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Math-Italic.ttf": "a7732ecb5840a15be39e1eda377bc21d",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_SansSerif-Bold.ttf": "ad0a28f28f736cf4c121bcb0e719b88a",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_SansSerif-Italic.ttf": "d89b80e7bdd57d238eeaa80ed9a1013a",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_SansSerif-Regular.ttf": "b5f967ed9e4933f1c3165a12fe3436df",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Script-Regular.ttf": "55d2dcd4778875a53ff09320a85a5296",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Size1-Regular.ttf": "1e6a3368d660edc3a2fbbe72edfeaa85",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Size2-Regular.ttf": "959972785387fe35f7d47dbfb0385bc4",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Size3-Regular.ttf": "e87212c26bb86c21eb028aba2ac53ec3",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Size4-Regular.ttf": "85554307b465da7eb785fd3ce52ad282",
"assets/packages/flutter_math_fork/lib/katex_fonts/fonts/KaTeX_Typewriter-Regular.ttf": "87f56927f1ba726ce0591955c8b3b42d",
"assets/packages/wakelock_web/assets/no_sleep.js": "7748a45cd593f33280669b29c2c8919a",
"favicon.png": "a36fab7c0b99034ede1f9ce707f578b7",
"firebase-messaging-sw.js": "82e5281f8978367fad5d4689d50650d7",
"icons/Icon-192.png": "48a0ace306cef98c5d51f89280850889",
"icons/Icon-512.png": "d70812f06169d43a864da7bc35447790",
"index.html": "366cbfcae7baa68a4b81a4a5b119239d",
"/": "366cbfcae7baa68a4b81a4a5b119239d",
"main.dart.js": "85cd7ed74acd846d953840e5ec2b572a",
"manifest.json": "8d9cf08c743b10cca6b724b03789a842",
"version.json": "02f67ffd0764298b5e5a6c2dcecc36f9"
};

// The application shell files that are downloaded before a service worker can
// start.
const CORE = [
  "/",
"main.dart.js",
"index.html",
"assets/NOTICES",
"assets/AssetManifest.json",
"assets/FontManifest.json"];
// During install, the TEMP cache is populated with the application shell files.
self.addEventListener("install", (event) => {
  self.skipWaiting();
  return event.waitUntil(
    caches.open(TEMP).then((cache) => {
      return cache.addAll(
        CORE.map((value) => new Request(value, {'cache': 'reload'})));
    })
  );
});

// During activate, the cache is populated with the temp files downloaded in
// install. If this service worker is upgrading from one with a saved
// MANIFEST, then use this to retain unchanged resource files.
self.addEventListener("activate", function(event) {
  return event.waitUntil(async function() {
    try {
      var contentCache = await caches.open(CACHE_NAME);
      var tempCache = await caches.open(TEMP);
      var manifestCache = await caches.open(MANIFEST);
      var manifest = await manifestCache.match('manifest');
      // When there is no prior manifest, clear the entire cache.
      if (!manifest) {
        await caches.delete(CACHE_NAME);
        contentCache = await caches.open(CACHE_NAME);
        for (var request of await tempCache.keys()) {
          var response = await tempCache.match(request);
          await contentCache.put(request, response);
        }
        await caches.delete(TEMP);
        // Save the manifest to make future upgrades efficient.
        await manifestCache.put('manifest', new Response(JSON.stringify(RESOURCES)));
        return;
      }
      var oldManifest = await manifest.json();
      var origin = self.location.origin;
      for (var request of await contentCache.keys()) {
        var key = request.url.substring(origin.length + 1);
        if (key == "") {
          key = "/";
        }
        // If a resource from the old manifest is not in the new cache, or if
        // the MD5 sum has changed, delete it. Otherwise the resource is left
        // in the cache and can be reused by the new service worker.
        if (!RESOURCES[key] || RESOURCES[key] != oldManifest[key]) {
          await contentCache.delete(request);
        }
      }
      // Populate the cache with the app shell TEMP files, potentially overwriting
      // cache files preserved above.
      for (var request of await tempCache.keys()) {
        var response = await tempCache.match(request);
        await contentCache.put(request, response);
      }
      await caches.delete(TEMP);
      // Save the manifest to make future upgrades efficient.
      await manifestCache.put('manifest', new Response(JSON.stringify(RESOURCES)));
      return;
    } catch (err) {
      // On an unhandled exception the state of the cache cannot be guaranteed.
      console.error('Failed to upgrade service worker: ' + err);
      await caches.delete(CACHE_NAME);
      await caches.delete(TEMP);
      await caches.delete(MANIFEST);
    }
  }());
});

// The fetch handler redirects requests for RESOURCE files to the service
// worker cache.
self.addEventListener("fetch", (event) => {
  if (event.request.method !== 'GET') {
    return;
  }
  var origin = self.location.origin;
  var key = event.request.url.substring(origin.length + 1);
  // Redirect URLs to the index.html
  if (key.indexOf('?v=') != -1) {
    key = key.split('?v=')[0];
  }
  if (event.request.url == origin || event.request.url.startsWith(origin + '/#') || key == '') {
    key = '/';
  }
  // If the URL is not the RESOURCE list then return to signal that the
  // browser should take over.
  if (!RESOURCES[key]) {
    return;
  }
  // If the URL is the index.html, perform an online-first request.
  if (key == '/') {
    return onlineFirst(event);
  }
  event.respondWith(caches.open(CACHE_NAME)
    .then((cache) =>  {
      return cache.match(event.request).then((response) => {
        // Either respond with the cached resource, or perform a fetch and
        // lazily populate the cache.
        return response || fetch(event.request).then((response) => {
          cache.put(event.request, response.clone());
          return response;
        });
      })
    })
  );
});

self.addEventListener('message', (event) => {
  // SkipWaiting can be used to immediately activate a waiting service worker.
  // This will also require a page refresh triggered by the main worker.
  if (event.data === 'skipWaiting') {
    self.skipWaiting();
    return;
  }
  if (event.data === 'downloadOffline') {
    downloadOffline();
    return;
  }
});

// Download offline will check the RESOURCES for all files not in the cache
// and populate them.
async function downloadOffline() {
  var resources = [];
  var contentCache = await caches.open(CACHE_NAME);
  var currentContent = {};
  for (var request of await contentCache.keys()) {
    var key = request.url.substring(origin.length + 1);
    if (key == "") {
      key = "/";
    }
    currentContent[key] = true;
  }
  for (var resourceKey of Object.keys(RESOURCES)) {
    if (!currentContent[resourceKey]) {
      resources.push(resourceKey);
    }
  }
  return contentCache.addAll(resources);
}

// Attempt to download the resource online before falling back to
// the offline cache.
function onlineFirst(event) {
  return event.respondWith(
    fetch(event.request).then((response) => {
      return caches.open(CACHE_NAME).then((cache) => {
        cache.put(event.request, response.clone());
        return response;
      });
    }).catch((error) => {
      return caches.open(CACHE_NAME).then((cache) => {
        return cache.match(event.request).then((response) => {
          if (response != null) {
            return response;
          }
          throw error;
        });
      });
    })
  );
}

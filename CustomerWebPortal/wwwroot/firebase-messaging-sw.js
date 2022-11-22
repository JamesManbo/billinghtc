importScripts("https://www.gstatic.com/firebasejs/8.6.1/firebase-app.js");
importScripts("https://www.gstatic.com/firebasejs/8.6.1/firebase-messaging.js");


firebase.initializeApp({
apiKey: "AIzaSyBltynlOdMHUmqmP2zzqUnsqfp9IJ9-mxk",
  authDomain: "billing-staff.firebaseapp.com",
  databaseURL: "https://billing-staff.firebaseio.com",
  projectId: "billing-staff",
  storageBucket: "billing-staff.appspot.com",
  messagingSenderId: "1098918185200",
  appId: "1:1098918185200:web:8f2c2c748962b4d36d1a35",
  measurementId: "G-3297P8WPR0"
});
// Necessary to receive background messages:
const messaging = firebase.messaging();
// Optional:
messaging.onBackgroundMessage((m) => {
  console.log("onBackgroundMessage", m);
});
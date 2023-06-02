import React from "react";
import ReactDOM from "react-dom/client";
import App from "./app/app";
import "@grapecity/ar-viewer/dist/jsViewer.min.js";
import "@grapecity/ar-viewer/dist/jsViewer.min.css";

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<App />);
# ESP32 Scanner API & User App

This project provides an **API backend** and a **user-facing web app** for visualizing data collected by the [ESP32 Wi-Fi Probe Request Scanner](https://github.com/Rev0lt0x33/ESP32_Scanner).  

The ESP32 scanner sends **MAC addresses of nearby devices** to the API server.  
The backend stores this data in a **PostgreSQL database**, and the web app displays:
- A list of detected devices
- Charts of how many **unique MACs** were seen over time
- Timeline of detections
- Manage API keys and scanner devices  

---

## Features
- **User accounts** with login & authentication  
- **API key management**: each user can generate key to link ESP32 scanners  
- **ESP32 → API** communication secured via API key  
- **PostgreSQL database** for storing data and user/device mappings  
- **Web dashboard** showing:
  - List of detected devices per user
  - List of scanners
  - Unique MAC count per time interval  
  - Historical charts  

## Disclaimer
The system only store information that is already present in the standard  
**IEEE 802.11 probe request frame**, such as:
- Device MAC address  
- Timestamp of detection  


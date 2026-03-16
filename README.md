# LeyKarin Denuncias — App Móvil

> Aplicación móvil **Android / iOS** para el envío confidencial de denuncias según la **Ley N° 21.643 (Ley Karin)** — Chile

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-10.0-512BD4?logo=dotnet)
![Android](https://img.shields.io/badge/Android-7.0%2B-3DDC84?logo=android)
![iOS](https://img.shields.io/badge/iOS-15%2B-000000?logo=apple)
![License](https://img.shields.io/badge/licencia-privada-red)

---

## ¿Para qué sirve?

Permite que los trabajadores de una empresa puedan **enviar denuncias de acoso laboral, acoso sexual o maltrato** directamente desde su teléfono, de forma anónima o identificada, sin necesidad de acceder a un computador.

La app se conecta al servidor [LeyKarin Platform](https://github.com/sfherrera/leykarin) de la empresa y registra la denuncia en el sistema oficial.

---

## Capturas de pantalla

| Inicio | Tipo de denuncia | Detalles | Confirmación |
|--------|-----------------|---------|--------------|
| Conectar empresa con URL + código | Selección visual del tipo | Formulario de detalles | Folio generado |

---

## Características

| Función | Descripción |
|---------|-------------|
| 🔗 **Conexión flexible** | Se configura con la URL del servidor y el código de empresa |
| 📋 **Wizard guiado** | 3 pasos: tipo → detalles → revisión |
| 🎭 **Denuncia anónima** | El denunciante puede no revelar su identidad |
| 👤 **Denuncia identificada** | Incluye datos de contacto opcionales |
| 👥 **Datos del denunciado** | Nombre, cargo y relación con el denunciante |
| 📁 **Folio de seguimiento** | Número único para hacer seguimiento del caso |
| 💾 **Configuración persistente** | Recuerda la empresa configurada entre sesiones |
| 🔒 **Confidencial** | Cumple con la Ley N° 21.643 |

---

## Tecnología

- **Framework:** .NET 10 MAUI Blazor Hybrid
- **UI:** Blazor (Razor Components) + CSS nativo
- **Plataformas:** Android 7.0+ (API 24) y iOS 15+
- **Comunicación:** HTTP/HTTPS REST API al servidor LeyKarin
- **Almacenamiento local:** `Preferences` (SharedPreferences en Android / NSUserDefaults en iOS)

---

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Workload MAUI instalado:
  ```bash
  dotnet workload install maui
  ```
- Para Android: Android SDK (API 24+)
- Para iOS: macOS con Xcode 15+
- Servidor **LeyKarin Platform** en ejecución (ver [repositorio principal](https://github.com/sfherrera/leykarin))

---

## Instalación y desarrollo

### 1. Clonar el repositorio

```bash
git clone https://github.com/sfherrera/leykarinapp.git
cd leykarinapp
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Ejecutar en emulador Android

```bash
dotnet build -f net10.0-android
dotnet run -f net10.0-android
```

### 4. Ejecutar en simulador iOS (requiere macOS)

```bash
dotnet run -f net10.0-ios
```

---

## Generar APK (Android)

### Debug (para pruebas)

```bash
dotnet build -f net10.0-android -c Debug
```

El APK se genera en:
```
bin/Debug/net10.0-android/cl.leykarin.denuncias-Signed.apk
```

### Release (para distribución)

```bash
dotnet publish -f net10.0-android -c Release
```

> ⚠️ Para publicar en Google Play necesitas configurar una clave de firma (keystore). Ver documentación de [MAUI Android signing](https://learn.microsoft.com/dotnet/maui/android/deployment/publish-cli).

---

## Generar IPA (iOS)

```bash
dotnet publish -f net10.0-ios -c Release
```

> ⚠️ Requiere cuenta de Apple Developer y perfil de distribución.

---

## Configuración de la app

Al abrirla por primera vez, el trabajador debe ingresar:

| Campo | Ejemplo | Descripción |
|-------|---------|-------------|
| **URL del servidor** | `https://demo.leykarin.cl` | URL completa del servidor LeyKarin de tu empresa |
| **Código de empresa** | `demo` | Slug de la empresa (lo provee el administrador) |

La URL pública para cada empresa tiene el formato:
```
https://tudominio.cl/denuncia/{codigo-empresa}
```

Esta misma URL puede distribuirse como código QR en la empresa — la app puede usar el mismo código.

---

## Estructura del proyecto

```
LeyKarinApp/
├── Models/
│   ├── TipoDenunciaDto.cs       — Tipos de denuncia disponibles
│   ├── EmpresaInfoDto.cs        — Información de la empresa conectada
│   ├── DenunciaRequest.cs       — Datos del formulario de denuncia
│   └── DenunciaResponse.cs      — Respuesta con folio generado
├── Services/
│   ├── ConfiguracionService.cs  — Guarda URL y slug del dispositivo
│   └── DenunciaApiService.cs    — Cliente HTTP al backend
├── Components/
│   ├── Pages/
│   │   ├── Home.razor           — Pantalla de inicio y conexión
│   │   ├── NuevaDenuncia.razor  — Wizard de 3 pasos
│   │   └── Confirmacion.razor   — Éxito con número de folio
│   └── Layout/
│       └── MainLayout.razor     — Layout sin barra de navegación
├── Platforms/
│   ├── Android/
│   │   ├── AndroidManifest.xml  — Permisos (INTERNET, cleartext HTTP)
│   │   └── MainActivity.cs
│   └── iOS/
│       ├── Info.plist
│       └── AppDelegate.cs
├── wwwroot/
│   ├── app.css                  — Estilos mobile-first
│   └── index.html               — Punto de entrada Blazor WebView
└── LeyKarinApp.csproj
```

---

## API del servidor requerida

La app consume estos endpoints del servidor LeyKarin (todos anónimos):

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/portal/{slug}/info` | Nombre y colores de la empresa |
| `GET` | `/api/portal/{slug}/tipos` | Lista de tipos de denuncia |
| `POST` | `/api/portal/{slug}/denuncia` | Envía una nueva denuncia |

Estos endpoints están disponibles a partir de la versión del servidor que incluye el soporte para app móvil (`Canal = AppMovil`).

---

## Seguridad

- Las denuncias anónimas **no almacenan ningún dato del denunciante**
- La comunicación con el servidor debe ser por **HTTPS en producción**
- Los datos se transmiten directamente al servidor, sin intermediarios
- No se almacena ninguna denuncia en el dispositivo

---

## Distribución interna (sin Google Play)

Para distribuir el APK directamente a los trabajadores sin pasar por Google Play:

1. Genera el APK de release:
   ```bash
   dotnet publish -f net10.0-android -c Release
   ```
2. Sube el APK a un servidor interno o comparte por correo/intranet
3. El trabajador debe habilitar **"Instalar apps de fuentes desconocidas"** en su Android
4. Instala el APK desde el archivo descargado

---

## Licencia

Software privado. Todos los derechos reservados © 2025.

Parte del ecosistema **LeyKarin Platform** — [github.com/sfherrera/leykarin](https://github.com/sfherrera/leykarin)

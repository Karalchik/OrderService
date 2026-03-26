Инструкция по запуску OrderService
Предварительные требования
Перед запуском убедитесь, что у вас установлены:

Docker Desktop.

.NET 8 SDK.

Быстрый старт (Docker)

Откройте терминал в корневой папке проекта (где файл docker-compose.yml).

Выполните команду:

docker-compose up --build
Дождитесь завершения сборки. Когда в консоли появятся логи приложения, сервис готов к работе.

Доступ к сервису:
Swagger UI: http://localhost:5000/swagger — основная панель для тестирования запросов.

API Endpoint: http://localhost:5000/api/v1/orders

Тестирование

Запуск через CLI:

dotnet test
Запуск через Visual Studio:
Откройте Test Explorer и нажмите Run All.


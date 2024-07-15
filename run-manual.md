Dockerfile-ы собрать не успел, так что придется кое-кому ручками поработать.

#### Step-by-step manual for local demo:
1. Установить postgres любой версии, либо в контейнере.
2. В проекте строку подключения натравить на ваш инстанс постгреса.
3. Собрать проект и запустить Todos.Api.dll проект через команду dotnet, с указанием порта 5146 (localhost:5146).
4. Frontend:    

    4.1 Установить nodejs под Angular v18  
    4.2 Установить ng cli  
    4.3 Перейти в директорию ClientApp проекта Todos.Spa  
    4.4 Запустить команду npm install  
    4.5 Запустить команду ng serve --open  
    4.6 PROFIT - Запустится фронт на Development Server-е.  
5. Поскольку front не поддерживает регистрацию новых пользователей, то
   войти можно только под заранее созданными пользователями (email/password):

            - uzumaki@ryansoftware.com/narutoSecret  
            - valeraadmin@ryansoftware.com/secret
6. Далее можно тестировать функционал через SPA, либо в SwaggerUI - кому как угодно.

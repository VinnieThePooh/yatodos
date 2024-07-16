# yatodos
Damn, yet another todo list)

#### Step-by-step manual for local demo:
1. Установить docker, docker-compose на рабочую машину.
    + Если вы на Windows/Mac-машине, то достаточно поставить Docker Desktop.
    + Если же на Linux-е, то docker, docker-compose ставятся отдельно.
2. Подготовка фронта (опционально, если хочется глянуть на то как back верстает :blush:)

    2.1 Открыть терминал и перейти в директорию фронта - `Todos.Spa/ClientApp`    
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Далее все команды выполняются в терминале.

    2.2 Установить nodejs под Angular v18   (в зависимости от ОС инструкции разные).

    2.3 Установить ng cli:    
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<code>npm i -g @angular/cli@latest</code>

    2.4 Установить наш SPA-package:    
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<code>`npm install`</code>
        
    2.5 Развернуть Angular-статику используя ng cli:    
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<code>ng build --configuration production</code>
    
3. Если предыдущий шаг был пропущен, то требуется в docker-compose файле закомментировать секцию `todos.spa` полностью.
4. В терминале перейти в директорию с docker-compose файлом и запустить наш multi-container app:
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<code>docker-compose up</code>
5. Реквизиты по-умолчанию:
    + Адреса:
        + `http://localhost:8080` - SPA-frontend
        + `http://localhost:5147` - Todos.API
    
    + Имена/Пароли. Регистрация новых пользователей не предусмотрена,   
    поэтому используем заранее сидированные.
        + `uzumaki@ryansoftware.com`/`narutoSecret`
        + `valeraadmin@ryansoftware.com`/`secret`
        

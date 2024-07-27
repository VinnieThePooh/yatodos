# to run:
# docker build -t todos.spa [--no-cache] -f ./Todos.Spa/Dockerfile ./Todos.Spa/ClientApp/ - in solution dir
#build stage for a Node.js application
FROM node:lts-alpine as build-stage
WORKDIR /app
COPY ./package*.json ./  
RUN npm install -g npm@latest && npm install
RUN npm install -g @angular/cli
COPY ./* ./
RUN ng build --configuration production # - FAILED HERE!!
COPY dist/browser/* /app/dist

# делаем ход конем, предварительно выполнив команду ng build --configuration production у себя локально
FROM nginx:stable-alpine as production-stage
#
#COPY ./dist/client-app/browser/*  /usr/share/nginx/html/
COPY --from=build-stage /app/dist/browser/*  /usr/share/nginx/html/
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
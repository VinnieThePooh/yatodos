# to run:
# docker build -t todos.spa [--no-cache] -f ./Todos.Spa/Dockerfile ./Todos.Spa/ClientApp/ - in solution dir
#build stage for a Node.js application
FROM node:lts-alpine as build-stage
ARG appPrefix="app"
RUN npm i -g npm@latest && npm i -g @angular/cli
RUN ng new app  \
    --prefix ${appPrefix} \
    --skip-install \
    --skip-tests \
    --skip-git \
    --style css \
    --ssr false \
    --skip-git \
    --minimal \
    --standalone
WORKDIR /app/
ENTRYPOINT ["sh", "-c", "tail -f /dev/null"]
#remove cli generated assets and replace by ours
RUN rm *.json && \
    rm -rf public src
COPY angular.json package.json tsconfig.json tsconfig.app.json tsconfig.spec.json public src ./
RUN npm i
##build output: /app/dist/    but we need to dive deeper: /dist/browser/*
RUN ng build --delete-output-path --output-path=/dist -c production
RUN ls /dist
#ENTRYPOINT ["sh", "-c", "tail -f /dev/null"]
         
#COPY ./*.json ./
#RUN rm package-lock.json
#RUN ls ./
#RUN npm i
#RUN pwd && ls ./ 
#COPY ./public/ ./src/ ./
#RUN ls ./
##ENTRYPOINT ["sh", "-c", "tail -f /dev/null"]
#RUN ng build --delete-output-path -c production # - FAILED HERE!!
#ENTRYPOINT ["sh", "-c", "tail -f /dev/null"]
#COPY dist/browser/* /app/dist

# делаем ход конем, предварительно выполнив команду ng build --configuration production у себя локально
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /dist/browser/*  /usr/share/nginx/html/
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
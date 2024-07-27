# to run:
# docker build -t todos.spa [--no-cache] -f ./Todos.Spa/Dockerfile ./Todos.Spa/ClientApp/ - in solution dir
#build stage for a Node.js application
FROM node:lts-alpine as build-stage
RUN npm i -g npm@latest && npm i -g @angular/cli
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
CMD ["ng", "serve", "--host", "0.0.0.0"]
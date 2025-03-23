docker-build:
	docker build -t diff.api .

docker-run:
	docker run --rm --network host -e Urls="http://localhost:5000" diff.api

all: docker-build docker-run

debug-docker-build:
	docker build -t diff.api.debug -f Dockerfile-debug .

debug-docker-run:
	docker run --rm --network host -e Urls="http://localhost:5000" diff.api.debug

debug: debug-docker-build debug-docker-run
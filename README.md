# Deploy DAPR to Kubernetes

- [x] Pub / Sub
- [x] State Store

## Local Debug

> Just set the `docker-compose` as startup project and start debugging

### Pre-requisites

#### Install Docker

```ps1
winget install -e Docker.DockerDesktop
```

#### Install DAPR

```ps1
winget install -e Dapr.CLI
```

---

## Deploy to Kubernetes

> Instructions are specific to `minikube`, but can be applied to any other kubernetes environment

### Pre-requisites

#### Install Minikube

```ps1
winget install -e Kubernetes.minikube
```

#### Install Helm

```ps1
winget install -e Helm.Helm
```

#### Start minikube

```ps1
minikube start
```

#### Initialize dapr on kubernetes cluster

```ps1
dapr init -k
```

#### Install Redis on the cluster

```ps1
helm install redis bitnami/redis --set auth.enabled=false
```

#### Declare the redis instance as the dapr components (statestore & pubsub)

```ps1
kubectl apply -f ./deployment/dapr-components.yaml
```

#### Build docker images locally

```ps1
docker build -f .\Publisher\Dockerfile -t spicycoder/publisher:latest .
docker build -f .\Subscriber\Dockerfile -t spicycoder/subscriber:latest .
```

#### Push docker images locally

```ps1
docker push spicycoder/publisher:latest
docker push spicycoder/subscriber:latest
```

> Replace `spicycoder` with your docker.io username

#### Install Publisher on the cluster

```ps1
kubectl apply -f ./deployment/publisher-deployment.yaml
```

#### Install Subscriber on the cluster

```ps1
kubectl apply -f ./deployment/subscriber-deployment.yaml
```

#### Port forward Publisher

```ps1
kubectl port-forward service/publisher 8080:8181
```

> Visit <http://localhost:8080/swagger/index.html> and try saving + reading states.

> You can also try publishing a message and check logs through minikube dashboard, that the message was received by the Subscriber application

```ps1
minikube dashboard
```

> If interested, you can also check dapr dashboard at <http://localhost:1818>

```ps1
dapr dashboard -k -p 1818
```

---

## RabbitMQ Component

Replace `components/pubsub.yaml` with below content, to use RabbitMQ instead of Redis Streams for pub / sub

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: pubsub
  namespace: default
spec:
  type: pubsub.rabbitmq
  version: v1
  metadata:
  - name: host
    value: "amqp://rabbitmq:5672"
```

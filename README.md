# Deploy DAPR to Kubernetes

- [x] Pub / Sub
- [x] State Store

## Local Debug

> Ensure you have Docker Desktop is up and running and have [dapr initialized](https://docs.dapr.io/getting-started/install-dapr-selfhost/)

Just set the `docker-compose` as startup project and start debugging

## Deploy to Kubernetes

Start minikube

```ps1
minikube start
```

Initialize dapr on kubernetes cluster

```ps1
dapr init -k
```

Install Redis on the cluster

```ps1
kubectl apply -f ./deployment/dapr-components.yaml
```

Install Publisher on the cluster

```ps1
kubectl apply -f ./deployment/publisher-deployment.yaml
```

Install Subscriber on the cluster

```ps1
kubectl apply -f ./deployment/subscriber-deployment.yaml
```

Port forward Publisher

```ps1
kubectl port-forward service/publisher 8080:8181
```

Visit <http://localhost:8080/swagger/index.html> and try saving + reading states.

You can also try publishing a message and check logs through minikube dashboard, that the message was received by the Subscriber application

```ps1
minikube dashboard
```

If interested, you can also check dapr dashboard at <http://localhost:1818>

```ps1
dapr dashboard -k -p 1818
```

## RabbitMQ Component

Replace `components/pubsub.yaml` with

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

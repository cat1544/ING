steps:
  # Docker Build
  - name: 'gcr.io/cloud-builders/docker'
    args: ['build', '-t',
           'asia-northeast3-docker.pkg.dev/${PROJECT_ID}/gcf-artifacts/my-image:latest',
           './k8s-manifests']

  # Docker Push
  - name: 'gcr.io/cloud-builders/docker'
    args: ['push',
           'asia-northeast3-docker.pkg.dev/${PROJECT_ID}/gcf-artifacts/my-image:latest']

  # Change manifests images version
  - name: 'gcr.io/cloud-builders/gcloud'
    entrypoint: 'sed'
    args: ['-i', 's|DOCKER_IMAGE|asia-northeast3-docker.pkg.dev/${PROJECT_ID}/gcf-artifacts/my-image:latest|', 'k8s-manifests/test.yaml']


options:
  logging: CLOUD_LOGGING_ONLY
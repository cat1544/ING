# Cloudbuild 설명

## 태그 기반 트리거
```
# 태그 푸쉬 전 도커파일을 수정후 레포에 푸쉬를 해 둔 상태에서

# 서비스 v1.0.0 태그 생성
git tag servicev1.0.0

# 서비스 태그 푸쉬
git push origin servicev1.0.0

# 클라우드 빌드 트리거로 클라우드 빌드가 실행

# ING-k8s repo의 manifests.yaml에 서비스의 이름을 인식하여 image부분에 태그를 수정함

# 태그가 수정되면 레포를 바라보고 있던 argocd가 해당 부분의 deployment부분만 다시 배포해줌
```
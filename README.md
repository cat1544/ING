<p align="center">
<img src="/src/frontend/static/icons/Hipster_NavLogo.svg" width="300" alt="Online Boutique" />
</p>

# ING
GCP ING팀의 파이널 프로젝트 github입니다!

# github flow를 활용하여
<img src="/etc/github_flow.jpg"/>

github flow를 참고하여 인프라를 설계하였습니다.

서비스를 운영중인 상태에서 develop브랜치를 생성하고 업데이트 및 버그패치후 main브랜치로 PR하여 merge를 하는 방식입니다.

소스 코드는 [GCP의 boutique](https://github.com/GoogleCloudPlatform/microservices-demo)를 참고하여 작성하였고, IaC기반 CICD를 구현한 MSA운영 인프라 구축을 목표로 하였습니다.

인프라 레포의 주소는 다음과 같습니다. [ING_k8s](https://github.com/cat1544/ING_k8s/tree/main) (프라이빗 레포라 들어가진 못해요... 필요하다면 이메일로 보내드립니다!)

# cloudbuild.yaml파일
<img src="/etc/cloudbuild_trigger.jpg"/>

각 서비스에 cloudbuild.yaml파일이 있습니다. cloudbuild의 trigger는 브랜치기반 push트리거를 기반으로 설정하여 해당 서비스의 src폴더 내의 모든 파일중 하나라도 수정하여 push를 하면 build가 진행되도록 하였습니다.


# git tag 하는법
git tag -a v1.0.0 -m"Release version 1.0.0"
git push origin v1.0.0

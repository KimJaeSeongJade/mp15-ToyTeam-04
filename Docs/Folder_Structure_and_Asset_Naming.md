# 폴더 구조와 에셋 이름

파일을 새로 만들 때 **어디에 넣을지**와 **어떤 이름을 붙일지**에 대한 규칙.

---

## Assets 폴더 구조

```
Assets/
	Scripts/
		Core/           게임 매니저, 싱글톤, 공통 시스템
		Player/         상호작용, 카메라
		Tower/          타워
		Monster/        이동(Nav Mesh), 체력, 공격
		Map/            스테이지에 따라 맵 변경
		UI/             Window, PopUp
		Audio/          사운드 재생 관리
	Scenes/
		Title.unity     타이틀 -> 인게임 전환시 Fade In/Out & 비동기 씬 전환
		InGame.unity
		Test/           개인 테스트 씬 (git아이디_Test)
	Prefabs/
		Tower/
		Monster/
		Map/
		UI/
		Etc/ 
		Test/           개인 폴더 (git아이디_Test)
	Image/
	  UI/
	Materials/
	Animations/
		Tower/
		Monster/
		UI/
	Audio/
		BGM/
		SFX/
	Effect/
	  Player/
	  Tower/
	  Monster/
	  Map/
	  UI/
	Fonts/
	Imports/            외부에서 받은 에셋
```

## 이름 규칙

- **영어**만 사용
- **PascalCase**를 기본으로 합니다. (`PlayerController`, `MonsterPatrol`)
- 공백과 하이픈을 쓰지 않기. 구분이 필요하면 언더바 사용.
- 에셋 종류를 접두사로 구분합니다.

| 종류 | 접두사 | 예시 |
| --- | --- | --- |
| 스크립트 | 없음 | `PlayerController.cs` |
| 씬 | 없음 | `InGame.unity` |
| 프리팹 | `PFB_` | `PFB_MonsterBasic.prefab` |
| 머티리얼 | `MAT_` | `MAT_PlayerBody.mat` |
| 텍스처 | `TEX_` | `TEX_GroundTile.png` |
| 애니메이션 클립 | `ANIM_` | `ANIM_PlayerRun.anim` |
| 애니메이터 컨트롤러 | `AC_` | `AC_Monster.controller` |
| 배경음 | `BGM_` | `BGM_MainTheme.mp3` |
| 효과음 | `SFX_` | `SFX_PuzzleComplete.wav` |
| 폰트 | `FONT_` | `FONT_MainUI.ttf` |

### 변형이 있을 때

같은 대상의 변형은 접미사로 구분합니다.

```
PFB_Monster_Normal.prefab
PFB_Monster_Fast.prefab

MAT_Player_Default.mat
MAT_Player_Damaged.mat
```

### 나쁜 예시

```
player controller.cs        공백
player-jump.wav             하이픈
pfb_monster.prefab          접두사 소문자
MonsterBasic.prefab         접두사 없음
SFX_퍼즐완료.wav             한국어
test2_final_진짜최종.prefab   무엇인지 알 수 없음
```

## 외부 에셋은 예외

`Assets/Imports/` 안의 파일은 **원본 이름 그대로 둡니다.** 이름을 바꾸면 그 에셋이 내부에서 참조하는 연결이 끊어질 수 있고, 나중에 업데이트를 받을 때도 문제가 됩니다.

## 스크립트 이름

클래스 이름과 파일 이름을 같게 맞추기.
컴포넌트에서 오류 발생 가능성 多

```
PlayerController.cs  ->  public class PlayerController : MonoBehaviour
```

이름은 **무엇을 하는 것인지 드러나게** 짓기

```
MonsterVision.cs        (O) 몬스터 시야
PuzzleManager.cs        (O) 퍼즐 관리
Test.cs                 (X)
NewBehaviourScript.cs   (X)
Script1.cs              (X)
```
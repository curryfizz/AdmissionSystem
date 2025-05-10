# Design Patterns in the Admission System
## 1. Factory Pattern (RoomFactory)
- To decouple objection creation: separate the instatiation of ```Room``` objects from the rest of the system.
- Logic centralization: Rooms are created consistently
- Static methods ```CreateInitialRooms``` and ```CreateRoom``` handle creation.
- It can be extended to fetch rooms from a DB/config file without modifying client code.

## 2. Singleton Pattern (AdmissionSystem)
- Only one instance manages all admission centers.
- Prevents duplicate instances (no two centerIds will be the same).
- Private Constructor and ```Lazy<T>``` initliazation.
- Can be accessed only via ```AdmissionSystem.GetInstance()```

## 3. Observer Pattern (AdmissionCenter ↔ Room)
- Rooms react to seat assignment and centers do not need to know details.
- Rooms can attach/detach at runtime.
- Subject (```AdmissionCenter```) implements ```IRoomSubject``` with ```Attach```/```Detach```/```NotifyObservers```
- Observer (```Room```) implements ```IObserver``` with ```Update()```
- When state changes in one object (center) should trigger actions in others (rooms).

## 4. Strategy Pattern (ISeatAllocationStrategy)
- New strategies can be added without changing existing code.
- Interface ```ISeatAllocationStrategy``` defines ```SelectRoom()```
- Concrete strategy ```UtilizationAwareStrategy``` implements logic.

### Diagram to represent classes and interfaces:
![mermaid-diagram-2025-05-07-111520](https://github.com/user-attachments/assets/1c9fe74b-9dc0-4320-be14-0384ed1a5b29)

### Diagram to represent program flow
![image](https://github.com/user-attachments/assets/32c833d2-4fb5-4de6-aac7-7fe62b7bb281)


# Design Patterns in the Migration System:
## 1.  Strategy Pattern (ISeatAssignmentStrategy, ISelectionStrategy)
- Selection Strategy (```ISelectionStrategy```) defines how students are picked for migration (e.g., ```RankFirstStrategy```).
- Seat Assignment Strategy (```ISeatAssignmentStrategy```) defines how students are assigned to departments (e.g., ```InitialAssignmentStrategy```).
- New strategies (like different selection or seat assignment methods) without changing the core logic of the migration system.

## 2. Template Method Pattern (MigrationSystem Process)
- The method ```RunNextCall()``` provides the main flow of selecting and assigning students, while the specific selection and assignment logic is handled by the strategies.
- Provides a consistent migration flow but allows flexibility by delegating certain steps to the strategies.
  
### Diagram to represent classes and interfaces:
![mermaid-diagram-2025-05-10-173105](https://github.com/user-attachments/assets/95a328ff-321f-4ce1-a636-90a5a9d76cef)

### Diagram to represent program flow
![image](https://github.com/user-attachments/assets/0d5bef75-ee4e-41b2-a269-42ed9bb2415e)






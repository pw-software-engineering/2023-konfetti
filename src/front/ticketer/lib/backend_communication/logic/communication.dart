// Singleton class for communication
class BackendCommunication {
  static final BackendCommunication _singleton =
      BackendCommunication._internal();
  factory BackendCommunication() {
    return _singleton;
  }
  BackendCommunication._internal();
}

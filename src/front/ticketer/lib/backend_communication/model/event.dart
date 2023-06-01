import 'package:ticketer/backend_communication/model/sector.dart';

import 'event_status.dart';

class Event {
  String? id;
  String name;
  String description;
  String location;
  String date;
  List<Sector> sectors;
  EventStatus status;

  Event(
    this.id,
    this.name,
    this.description,
    this.location,
    this.date,
    this.sectors,
    this.status,
  );

  factory Event.fromJson(Map<String, dynamic> json) {
    List<Sector> sectors = _getSectorsFromJson(json);
    return Event(
      json['id'],
      json['name'],
      json['description'],
      json['location'],
      json['date'],
      sectors,
      EventStatus.values[json['status']],
    );
  }

  static List<Sector> _getSectorsFromJson(Map<String, dynamic> json) {
    List<Sector> sectors = List.empty(growable: true);
    for (var s in json['sectors']) {
      sectors.add(Sector.fromJson(s));
    }
    return sectors;
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'description': description,
        'location': location,
        'date': date,
        'sectors': sectors.map((item) => item.toJson()).toList(),
        'status': status.index,
      };
}

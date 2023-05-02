import 'package:ticketer/backend_communication/model/tax_type.dart';

class Organizer {
  String id;
  String companyName;
  String address;
  TaxType taxIdType;
  String taxId;
  String displayName;
  String email;
  String phoneNumber;


  Organizer(
      this.id,
      this.companyName,
      this.address,
      this.taxIdType,
      this.taxId,
      this.displayName,
      this.email,
      this.phoneNumber,
      );

  factory Organizer.fromJson(Map<String, dynamic> json) {
    return Organizer(
        json['id'],
        json['companyName'],
        json['address'],
        TaxType.values[json['taxIdType']],
        json['taxId'],
        json['displayName'],
        json['email'],
        json['phoneNumber']
    );
  }

  Map<String, dynamic> toJson() => {
    'id': id,
    'companyName': companyName,
    'address': address,
    'taxIdType': taxIdType.index,
    'taxId': taxId,
    'displayName': displayName,
    'email': email,
    'phoneNumber': phoneNumber,
  };
}

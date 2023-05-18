import 'package:ticketer/backend_communication/model/tax_type.dart';

class OrganizerAccount {
  String companyName;
  String address;
  TaxType taxIdType;
  String taxId;
  String displayName;
  String email;
  String password;
  String phoneNumber;

  OrganizerAccount(
    this.companyName,
    this.address,
    this.taxIdType,
    this.taxId,
    this.displayName,
    this.email,
    this.password,
    this.phoneNumber,
  );

  factory OrganizerAccount.fromJson(Map<String, dynamic> json) {
    return OrganizerAccount(
        json['companyName'],
        json['address'],
        TaxType.values[json['taxIdType']],
        json['taxId'],
        json['displayName'],
        json['email'],
        json['password'],
        json['phoneNumber']);
  }

  Map<String, dynamic> toJson() => {
        'companyName': companyName,
        'address': address,
        'taxIdType': taxIdType.index,
        'taxId': taxId,
        'displayName': displayName,
        'email': email,
        'password': password,
        'phoneNumber': phoneNumber,
      };
}

class OrganizerAccountUpdate {
  String companyName;
  String address;
  TaxType taxIdType;
  String taxId;
  String displayName;
  String email;
  String phoneNumber;

  OrganizerAccountUpdate(
    this.companyName,
    this.address,
    this.taxIdType,
    this.taxId,
    this.displayName,
    this.email,
    this.phoneNumber,
  );

  factory OrganizerAccountUpdate.fromJson(Map<String, dynamic> json) {
    return OrganizerAccountUpdate(
      json['companyName'],
      json['address'],
      TaxType.values[json['taxIdType']],
      json['taxId'],
      json['displayName'],
      json['email'],
      json['phoneNumber'],
    );
  }

  Map<String, dynamic> toJson() => {
        'companyName': companyName,
        'address': address,
        'taxIdType': taxIdType.index,
        'taxId': taxId,
        'displayName': displayName,
        'email': email,
        'phoneNumber': phoneNumber,
      };
}

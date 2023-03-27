import 'package:ticketer/backend_communication/model/tax_type.dart';

class Organizer {
  String companyName;
  String address;
  TaxType taxIdType;
  String taxId;
  String displayName;
  String email;
  String password;
  String phoneNumber;

  Organizer(this.companyName, this.address, this.taxIdType, this.taxId,
      this.displayName, this.email, this.password, this.phoneNumber);

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

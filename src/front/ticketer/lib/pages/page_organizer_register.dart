import 'package:flutter/material.dart';

class OrganizerRegisterPage extends StatefulWidget {
  const OrganizerRegisterPage({Key? key}) : super(key: key);

  @override
  State<OrganizerRegisterPage> createState() => _OrganizerDataState();
}

class _OrganizerDataState extends State<OrganizerRegisterPage> {
  final TextEditingController _companyName = TextEditingController();
  final TextEditingController _addressCity = TextEditingController();
  final TextEditingController _addressZip = TextEditingController();
  final TextEditingController _addressStreet = TextEditingController();
  final TextEditingController _taxId = TextEditingController();
  final TextEditingController _displayName = TextEditingController();
  final TextEditingController _phone = TextEditingController();

  final _formKey = GlobalKey<FormState>();

  Widget _companyNameEntryField() {
    return TextFormField(
      controller: _companyName,
      decoration: const InputDecoration(
          labelText: "Company name", hintText: 'Enter your company name'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter company name";
        }
        return null;
      },
    );
  }

  Widget _companyCityEntryField() {
    return TextFormField(
      controller: _addressCity,
      decoration:
          const InputDecoration(labelText: "City", hintText: 'Enter city'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter city";
        }
        return null;
      },
    );
  }

  Widget _companyZipCodeEntryField() {
    return TextFormField(
      controller: _addressZip,
      decoration: const InputDecoration(
          labelText: "Postal / Zip code", hintText: 'Enter postal / zip code'),
      validator: (value) {
        if (value == null ||
            value.isEmpty ||
            !RegExp(r"^\d{2}-\d{3}$", caseSensitive: false).hasMatch(value)) {
          return "Please enter correct code in XX-XXX format";
        }
        return null;
      },
    );
  }

  Widget _companyStreetEntryField() {
    return TextFormField(
      controller: _addressStreet,
      decoration: const InputDecoration(
          labelText: "Address", hintText: 'Enter street and home number'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter address";
        }
        return null;
      },
    );
  }

  Widget _companyDisplayNameEntryField() {
    return TextFormField(
      controller: _displayName,
      decoration: const InputDecoration(
          labelText: "Display name", hintText: 'Enter displayed name'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter name";
        }
        return null;
      },
    );
  }

  Widget _companyTaxIdEntryField() {
    return TextFormField(
      controller: _taxId,
      decoration: const InputDecoration(
          labelText: "Tax id number",
          hintText: 'Enter tax identification number'),
      validator: (value) {
        if (value == null ||
            value.isEmpty ||
            !RegExp(r"^\d*$", caseSensitive: false).hasMatch(value)) {
          return "Please enter tax identification number";
        }
        return null;
      },
    );
  }

  Widget _companyPhoneEntryField() {
    return TextFormField(
      controller: _phone,
      decoration: const InputDecoration(
          labelText: "Phone", hintText: 'Enter phone number'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter phone number";
        }
        return null;
      },
    );
  }

  Widget _submitButton() {
    return Container(
        margin: const EdgeInsets.only(top: 15.0),
        child: ElevatedButton(
          onPressed: () => submitOrganizerData(),
          child: const Text("Submit"),
        ));
  }

  Widget _getRegisterContent() {
    return Form(
      key: _formKey,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: <Widget>[
          const Text(
            "Join us as an organizer",
            style: TextStyle(
                fontSize: 22,
                fontWeight: FontWeight.w600,
                color: Colors.blueAccent),
          ),
          _companyNameEntryField(),
          _companyCityEntryField(),
          _companyZipCodeEntryField(),
          _companyStreetEntryField(),
          _companyDisplayNameEntryField(),
          Row(
              mainAxisAlignment: MainAxisAlignment.center,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: <Widget>[
                const SizedBox(
                  child: TaxTypeDropdown(),
                ),
                const SizedBox(width: 10),
                SizedBox(
                  width: 260,
                  child: _companyTaxIdEntryField(),
                ),
              ]),
          _companyPhoneEntryField(),
          _submitButton(),
        ],
      ),
    );
  }

  Future<void> submitOrganizerData() async {
    if (_formKey.currentState!.validate()) {
      showDialog(
        context: context,
        builder: (context) {
          return AlertDialog(
            title: const Text("Thank you"),
            content: const Text(
                "Administrators has received your form. We will try to verify your data as soon as possible."),
            actions: [
              ElevatedButton(
                  onPressed: () => Navigator.pop(context),
                  child: const Text('OK')),
            ],
          );
        },
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Register as an organizer"),
      ),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 400),
          padding: const EdgeInsets.all(20),
          child: _getRegisterContent(),
        ),
      ),
    );
  }
}

class TaxTypeDropdown extends StatefulWidget {
  const TaxTypeDropdown({super.key});

  @override
  State<TaxTypeDropdown> createState() => _TaxTypeDropdownState();
}

class _TaxTypeDropdownState extends State<TaxTypeDropdown> {
  static const List<String> list = <String>[
    'NIP',
    'REGON',
    'KRS',
    'PESEL',
    'VATIN'
  ];
  String dropdownValue = list.first;

  @override
  Widget build(BuildContext context) {
    return DropdownButton<String>(
      value: dropdownValue,
      icon: const Icon(Icons.expand_more),
      style: const TextStyle(fontSize: 14),
      elevation: 16,
      underline: Container(
        color: const Color.fromARGB(255, 190, 190, 190),
        height: 1,
      ),
      onChanged: (String? value) {
        // This is called when the user selects an item.
        setState(() {
          dropdownValue = value!;
        });
      },
      items: list.map<DropdownMenuItem<String>>((String value) {
        return DropdownMenuItem<String>(
          value: value,
          child: Text(value),
        );
      }).toList(),
    );
  }
}

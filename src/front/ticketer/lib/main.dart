import 'package:ticketer/auth/widget_tree.dart';
import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';

import 'auth/auth.dart';

void main() async {
  await dotenv.load(fileName: "assets/dotenv");
  BackendCommunication().init();
  PaymentCommunication().init();
  Auth().init();

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return const MaterialApp(
      home: WidgetTree(),
    );
  }
}

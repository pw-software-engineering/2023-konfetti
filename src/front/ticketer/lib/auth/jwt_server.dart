import 'dart:convert';

import 'package:shelf_router/shelf_router.dart';
import 'package:shelf/shelf.dart';
import 'package:shelf/shelf_io.dart' as io;

void main() async {
  var app = Router();

  app.post('/account/login', (Request request) {
    var json = jsonEncode({
      'accessToken':
          'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiI2MTE2N2Y2NC0yYzI5LTQxN2MtYTcxNS0xMmExMzEzMDUzODMiLCJyb2xlIjoiT3JnYW5pemVyIiwibmJmIjoxNjc5NTE1NDA4LCJleHAiOjE2Nzk1Mjk4MDgsImlhdCI6MTY3OTUxNTQwOH0.Y5feFfliDFzQTKjHFwnRQ8m2uXygskEYg-dyAMSlabQ'
    });

    return Response.ok(json, headers: {'Access-Control-Allow-Origin': '*'});
  });

  await io.serve(app, 'localhost', 5166);
}

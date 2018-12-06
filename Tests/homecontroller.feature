Feature: Customer can view pages
       In order to get information
       As a customer
       I want to access the public pages

Scenario: Homepage
       When I go to page '/'
       Then the http result should be OK

Scenario: About
       When I go to page '/Home/About'
       Then the http result should be OK
       And the body contains 'Your application description page.'

Scenario: Contact
       When I go to page '/Home/Contact'
       Then the http result should be OK
       And the body contains 'Your contact page.'

Scenario: Privacy
       When I go to page '/Home/Privacy'
       Then the http result should be OK
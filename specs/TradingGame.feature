Feature: mvp

  Scenario: buy
    Given John Smith is identified
    Given he has 1000 on his account
    Given ORA is selling at 10 per share
    When he buys 100 shares of ORA
    Then he should get a confirmation of success
    Then his account balance should be 0
    Then his portfolio should contain 100 shares of ORA

  Scenario: sell
    Given John Smith is identified
    Given his portfolio contains 100 shares of ORA
    When he sells 100 shares of ORA
    Then he should get a confirmation of success
    Then his portfolio should contain 0 shares of ORA

  Scenario: 2 users
    Given Jane Doe is identified
    Given John Smith's portfolio contains 100 shares of ORA
    Given her portfolio contains 200 shares of ORA
    When she sells 100 shares of ORA
    Then she should get a confirmation of success
    Then her portfolio should contain 100 shares of ORA

  Scenario: reject short-sell
    Given Jane Doe is identified
    Given her portfolio contains 0 shares of TNT
    When she sells 100 shares of TNT
    Then she should be rejected
    Then her portfolio should contain 0 shares of TNT
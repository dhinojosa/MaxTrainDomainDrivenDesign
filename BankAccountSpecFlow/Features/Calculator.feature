Feature: This is going to be testing the bank account

    Scenario: Depositing money into an empty bank account
        Given an empty bank account
        When a deposit of 100 dollars is made
        Then then balance should be 100

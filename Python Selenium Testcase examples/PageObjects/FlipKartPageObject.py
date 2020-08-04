from selenium import webdriver
from selenium.webdriver.common.by import By

class FlipKartPageObject:
    def __init__(self, driver):
        self.driver = driver

    searchBar = (By.XPATH, "//div[@class='O8ZS_U']/input")

    def getSerachBar(self):
        return self.driver.find_element(*FlipKartPageObject.searchBar)
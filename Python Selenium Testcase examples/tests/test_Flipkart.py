from Classes.TestBase import TestBase
from PageObjects import FlipKartPageObject
import time

class TestFlipkart(TestBase):

    def test_FlipKart(self):
        self.driver.get("https://flipkart.com")
        fkPage = FlipKartPageObject.FlipKartPageObject(self.driver)
        searchBar = fkPage.getSerachBar()

        searchBar.send_keys("asd")
        time.sleep(100)
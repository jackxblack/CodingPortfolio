import pytest
from selenium import webdriver

driver = None

def pytest_addoption(parser):
    parser.addoption("--browser_name", action="store", default="chrome")

@pytest.fixture(scope="class")
def setup(request):
    global driver
    browser_name = request.config.getoption("browser_name")
    if browser_name == "chrome":
        driver = webdriver.Chrome(executable_path="C:\\Users\\Maciek\\Downloads\\chromedriver_win32\\chromedriver.exe")
    else:
        #I dont have other browsers installed, otherwise here I would just launch firefox or Edge based on text passed. For now I'll just launch chrome regardless
        driver = webdriver.Chrome(executable_path="C:\\Users\\Maciek\\Downloads\\chromedriver_win32\\chromedriver.exe")

    driver.maximize_window()
    request.cls.driver = driver
    yield
    driver.close()

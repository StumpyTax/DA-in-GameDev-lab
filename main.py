
import gspread
import numpy as np
import matplotlib.pyplot as plt

def model(a, b, x):
    return a*x + b

def loss_function(a, b, x, y):
    num = len(x)
    prediction=model(a, b, x)
    return (0.5/num) * (np.square(prediction-y)).sum()

def optimize(a, b, x, y):
    num = len(x)
    prediction = model(a, b, x)
    da = (1.0/num) * ((prediction -y)*x).sum()
    db = (1.0/num) * ((prediction -y).sum())
    a = a - Lr*da
    b = b - Lr*db
    return a, b

def iterate(a, b, x, y, times):
    for i in range(times):
        a,b = optimize(a,b,x,y)
    return a,b

a = np.random.rand(1)
b = np.random.rand(1)
Lr = 0.00001

gc = gspread.service_account(filename='lab2-365206-3534427906c2.json')

sh = gc.open("UnitySheets")

price = np.random.randint(2000, 10000, 11)
mon = list(range(1,12))

a,b = iterate(a,b,price,mon,100)
prediction = model(a,b,price)

i = 0
while i < len(mon):
    i += 1
    if i == 0:
        continue
    else:
        predInf=((prediction[i-1]-prediction[i-2])/prediction[i-2])*100
        tempInf = ((price[i-1]-price[i-2])/price[i-2])*100
        tempInf = str(tempInf)
        tempInf = tempInf.replace('.',',')
        predInf = str(predInf)
        predInf = predInf.replace('.',',')
        sh.sheet1.update(('A' + str(i)), str(i))
        sh.sheet1.update(('B' + str(i)), str(price[i-1]))
        sh.sheet1.update(('C' + str(i)), str(tempInf))
        sh.sheet1.update(('D' + str(i)), str(prediction[i-1]))
        sh.sheet1.update(('E' + str(i)), str(predInf))
        print(tempInf)
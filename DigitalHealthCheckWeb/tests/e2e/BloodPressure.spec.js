describe("Blood pressure page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/BloodPressure';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
       await expect(page).toMatchText("h1", "Blood pressure");
    })

    it("has expandable info for what is blood pressure ", async () => {
        const visible = await page.isVisible('text=Blood pressure is the pressure of blood in your arteries – the vessels that carry your blood from your heart to your brain and the rest of your body. You need a certain amount of pressure to get the blood moving around your body. High blood pressure is medically known as hypertension. It means that your blood pressure is consistently too high and your heart has to work harder to pump blood around your body. High blood pressure is serious. If you ignore it, it can lead to heart and circulatory diseases such as a heart attack or stroke. It can also cause kidney failure, heart failure, problems with your sight and vascular dementia.');
        expect(visible).toBeFalsy();
        // Click text=What is blood pressure and why does it matter?
        await page.click('text=What is blood pressure and why does it matter?');
        const visibleyes = await page.isVisible('text=Blood pressure is the pressure of blood in your arteries – the vessels that carry your blood from your heart to your brain and the rest of your body. You need a certain amount of pressure to get the blood moving around your body. High blood pressure is medically known as hypertension. It means that your blood pressure is consistently too high and your heart has to work harder to pump blood around your body. High blood pressure is serious. If you ignore it, it can lead to heart and circulatory diseases such as a heart attack or stroke. It can also cause kidney failure, heart failure, problems with your sight and vascular dementia.');
        expect(visibleyes).toBeTruthy();   

    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/BloodPressure')).toBeTruthy();
    })

    it("has systolic and diastolic fields if yes selected", async () => {
        // Click yes
        await page.click('#know-your-blood-pressure', 'yes');
        const checked = await page.isChecked('#know-your-blood-pressure', 'yes');
        expect(checked).toBeTruthy();
        // Fill input[name="Systolic"]
        await page.fill('input[name="Systolic"]', '119');
        // Fill input[name="Diastolic"]
        await page.fill('input[name="Diastolic"]', '82');

    })

    it("goes to cholesterol introduction when yes selected and bp entered", async () => {
        // Click yes
        await page.click('#know-your-blood-pressure', 'yes');
        // Fill input[name="BP"]
        await page.fill('input[name="Systolic"]', '126');
        // Fill input[name="BP"]
        await page.fill('input[name="Diastolic"]', '80');
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Cholesterol')).toBeTruthy();
        
    })

    it("does not require bp input if no selected", async () => {
        // Click no
        await page.click('#know-your-blood-pressure-3', 'no');
        const checked = await page.isVisible('input[name="BP"]');
        expect(checked).toBeFalsy();
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Cholesterol')).toBeTruthy();
    })

    it("should have a back link", async () => {
        // Click text=Back
        await page.click('text=Back');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/RiskFactors3')).toBeTruthy();
      })

  })  

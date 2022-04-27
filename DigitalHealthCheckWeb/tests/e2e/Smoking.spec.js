describe("Smoking page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoking';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Smoking");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url()).toBe('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoking');
        const error = await page.textContent('#error-summary-title');
        expect(error).toBe('There is a problem');
    })

    it("should select yes ", async () => {
        // Click #everSmoked
        await page.click('input[name="everSmoked"]', 'yes');
        const checked = await page.isChecked('input[name="everSmoked"]', 'yes');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Smoker')).toBeTruthy();
        await expect(page).toMatchText("h1", "Smoking page 2 of 2");
 
    })

    it("should go to alcohol question if never smoked ", async () => {
        // Click #everSmoked-2
        await page.click('#ever-smoked-2', 'no');        
        const checked = await page.isChecked('#ever-smoked-2', 'no');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DoYouDrinkAlcohol')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol");
        
    })

    it("should go to alcohol question if ex smoker ", async () => {
        // Click #everSmoked-3
        await page.click('#ever-smoked-3', 'ex');        
        const checked = await page.isChecked('#ever-smoked-3', 'ex');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DoYouDrinkAlcohol')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol");
        
    })

    it("should have a back link", async () => {
        // Click text=Back
        await page.click('text=Back');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Ethnicity')).toBeTruthy();
    })

})
describe("AUDIT1 page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT1';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // will be redirected to sex page
        await page.click('#sex-2', 'male');    
        await page.click('#identity', 'cis');          
        await page.click('button:has-text("Continue")');
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Alcohol page 3 of 7");
    })

    it("should not go onto next page if no option selected", async () => {
        // will be redirected to sex page
        await page.click('#sex-2', 'male');    
        await page.click('#identity', 'cis');          
        await page.click('button:has-text("Continue")');// Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT1')).toBeTruthy();
    })

    it("goes to physical activity if msasq negative", async () => {
        // will be redirected to sex page
        await page.click('#sex-2', 'male');    
        await page.click('#identity', 'cis');          
        await page.click('button:has-text("Continue")');
        // Fill input[name="Units"]
        await page.click('#units-2');
        await page.click('#msasq-2');
        const checked = await page.isChecked('#msasq-2', 'lessThanMonthly');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/PhysicalActivity')).toBeTruthy();
        await expect(page).toMatchText("h1", "Physical activity page 1 of 4");
 
    }) 

    it("goes to GPPAQ2 if msasq positive", async () => {
        // will be redirected to sex page
        await page.click('#sex-2', 'male');    
        await page.click('#identity', 'cis');          
        await page.click('button:has-text("Continue")');
        // Fill input[name="Units"]
        await page.click('#units-3');
        await page.click('#msasq-3');
        const checked = await page.isChecked('#msasq-3', 'monthly');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT2')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 4 of 7");
 
    }) 


})